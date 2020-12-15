SELECT run_command_on_workers('show ssl');
ALTER SYSTEM SET citus.node_conninfo TO 'sslmode=prefer';
SELECT pg_reload_conf();

SELECT create_distributed_table('tenants', 'id');
SELECT create_distributed_table('questions', 'tenant_id');

--
SELECT run_command_on_workers('show ssl');

SELECT * from master_add_node('localhost', 5432);

SELECT * from master_get_active_worker_nodes();

SELECT * from citus_remote_connection_stats();

SELECT * from pg_dist_shard_placement;

SELECT * from pg_dist_colocation;

SELECT * from pg_dist_placement;

SELECT * from pg_dist_shard;

-- SELECT * FROM get_rebalance_progress();
