-- Start event rollups
SELECT compute_rollups_every_5min((now()-interval '5 minutes')::timestamp, now()::timestamp);

-- Report example
SELECT sum(event_count), hll_cardinality(sum(device_distinct_count)) 
FROM rollup_events_5min where minute >=now()-interval '5 minutes' AND minute <=now() AND customer_id=1;