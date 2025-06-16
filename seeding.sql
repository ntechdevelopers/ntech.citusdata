INSERT INTO tenants VALUES (
    'c620f7ec-6b49-41e0-9913-08cfe81199af', 
    'ntechdevelopers.local',
    'Buffer Overflow',
    'Ask anything code-related!',
    now(),
    now());

INSERT INTO tenants VALUES (
    'b8a83a82-bb41-4bb3-bfaa-e923faab2ca4', 
    'api.ntechdevelopers.local',
    'Database Questions',
    'Figure out why your connection string is broken.',
    now(),
    now());

INSERT INTO questions VALUES (
    '347b7041-b421-4dc9-9e10-c64b8847fedf',
    'c620f7ec-6b49-41e0-9913-08cfe81199af',
    'How do you build apps in ASP.NET Core?',
    1,
    now(),
    now());

INSERT INTO questions VALUES (
    'a47ffcd2-635a-496e-8c65-c1cab53702a7',
    'b8a83a82-bb41-4bb3-bfaa-e923faab2ca4',
    'Using postgresql for multitenant data?',
    2,
    now(),
    now());