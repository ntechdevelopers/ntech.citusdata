SET citus.multi_task_query_log_level TO 'ERROR';
SELECT item_id, description FROM todo_items WHERE list_id = 1 ORDER BY position;


--
-- ERROR:  multi-task query about to be executed