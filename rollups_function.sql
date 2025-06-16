CREATE OR REPLACE FUNCTION compute_rollups_every_5min(start_time TIMESTAMP, end_time TIMESTAMP) RETURNS void LANGUAGE PLPGSQL AS $function$
BEGIN
  RAISE NOTICE 'Computing 5min rollups from % to % (excluded)', start_time, end_time;
 
RAISE NOTICE 'Aggregating data into 5 min rollup table';
EXECUTE $$
INSERT INTO rollup_events_5min
SELECT customer_id,
   event_type,
   country,
   browser,
   date_trunc('seconds', (timestamp - TIMESTAMP 'epoch') / 300) * 300 + TIMESTAMP 'epoch' AS minute,
   count(*) as event_count,
   hll_add_agg(hll_hash_bigint(device_id)) as device_distinct_count,
   hll_add_agg(hll_hash_bigint(session_id)) as session_distinct_count
FROM events WHERE timestamp >= $1 AND timestamp<=$2
GROUP BY
customer_id,
event_type,
country,
browser,
minute
ON CONFLICT (customer_id,event_type,country,browser,minute)
DO UPDATE
SET
   event_count=excluded.event_count,
   device_distinct_count = excluded.device_distinct_count,
   session_distinct_count= excluded.session_distinct_count;$$
USING start_time, end_time;
 
RAISE NOTICE 'Aggregating/Upserting into 1 hr rollup table';
EXECUTE $$
INSERT INTO rollup_events_1hr
SELECT customer_id,
   event_type,
   country,
   browser,
   date_trunc('hour', timestamp) as hour,
   count(*) as event_count,
   hll_add_agg(hll_hash_bigint(device_id)) as device_distinct_count,
   hll_add_agg(hll_hash_bigint(session_id)) as session_distinct_count
FROM events WHERE timestamp >= $1 AND timestamp<=$2
GROUP BY
customer_id,
event_type,
country,
browser,
hour
ON CONFLICT (customer_id,event_type,country,browser,hour)
DO UPDATE
SET
   event_count=rollup_events_1hr.event_count+excluded.event_count,
   device_distinct_count = rollup_events_1hr.device_distinct_count || excluded.device_distinct_count,
   session_distinct_count= rollup_events_1hr.session_distinct_count || excluded.session_distinct_count;$$
USING start_time, end_time;
END;
$function$;