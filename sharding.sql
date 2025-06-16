-- sharding
SELECT create_distributed_table('tenants', 'id');
SELECT create_distributed_table('questions', 'tenant_id');

SELECT create_reference_table('states');

-- get metadata
SELECT run_command_on_workers('show ssl');
ALTER SYSTEM SET citus.node_conninfo TO 'sslmode=prefer';
SELECT pg_reload_conf();

SELECT * from pg_dist_shard_placement;
SELECT * from pg_dist_colocation;
SELECT * from pg_dist_placement;
SELECT * from pg_dist_shard;

-- add master nodes
SELECT * from master_add_node('localhost', 5432);
SELECT * from master_get_active_worker_nodes();

-- add worker nodes
SELECT citus_add_node('10.0.0.2', 5432);
SELECT citus_add_node('10.0.0.3', 5432);
SELECT * from citus_remote_connection_stats();

-- rebalance the shards over the new worker nodes
SELECT rebalance_table_shards();
SELECT * from get_rebalance_progress();



