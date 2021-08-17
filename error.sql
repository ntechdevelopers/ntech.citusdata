SELECT list_id, list_name, num_items
FROM
  todo_lists lists
  JOIN
    (SELECT list_id, count(*) AS num_items 
    FROM todo_items 
    WHERE NOT done GROUP BY list_id) counts
  USING (list_id)
WHERE  user_id = 1
ORDER BY num_items DESC;

--
-- ERROR:  cannot pushdown the subquery since not all relations are joined using distribution keys