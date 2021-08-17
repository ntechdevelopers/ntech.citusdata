-- add master nodes
SELECT master_add_node('localhost', 5432);
SELECT citus_set_coordinator_host('localhost', 5432);

-- add worker nodes
SELECT citus_add_node('localhost', 5432);

-- rebalance the shards over the new worker nodes
SELECT rebalance_table_shards();

SELECT * from master_get_active_worker_nodes();

SELECT * from citus_remote_connection_stats();

---------------------------
CREATE TABLE events (
  device_id bigint,
  event_id bigserial,
  event_time timestamptz default now(),
  data jsonb not null,
  PRIMARY KEY (device_id, event_id)
);

-- distribute the events table across shards placed locally or on the worker nodes
SELECT create_distributed_table('events', 'device_id');

-- insert some events
INSERT INTO events (device_id, data)
SELECT s % 100, ('{"measurement":'||random()||'}')::jsonb FROM generate_series(1,1000000) s;

-- get the last 3 events for device 1, routed to a single node
SELECT * FROM events WHERE device_id = 1 ORDER BY event_time DESC, event_id DESC LIMIT 3;

-- insert some events
INSERT INTO events (device_id, data)
SELECT s % 100, ('{"measurement":'||random()||'}')::jsonb FROM generate_series(1,1000000) s;

-- get all
SELECT * FROM events

-- get the last 3 events for device 1, routed to a single node
SELECT * FROM events WHERE device_id = 1 ORDER BY event_time DESC, event_id DESC LIMIT 3;

-- Extract
EXPLAIN (VERBOSE ON) SELECT count(*) FROM events;