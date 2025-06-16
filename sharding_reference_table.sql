CREATE TABLE device_types (
  device_type_id int primary key,
  device_type_name text not null unique
);

-- replicate the table across all nodes to enable foreign keys and joins on any column
SELECT create_reference_table('device_types');

-- insert a device type
INSERT INTO device_types (device_type_id, device_type_name) VALUES (55, 'laptop');

-- optionally: make sure the application can only insert devices with known types
ALTER TABLE devices ADD CONSTRAINT device_type_fk
FOREIGN KEY (device_type_id) REFERENCES device_types (device_type_id);

-- get the last 3 events for devices whose type name starts with laptop, parallelized across shards
SELECT device_id, event_time, data->>'measurement' AS value, device_name, device_type_name
FROM events JOIN devices USING (device_id) JOIN device_types USING (device_type_id)
WHERE device_type_name LIKE 'laptop%' ORDER BY event_time DESC LIMIT 3;