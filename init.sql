CREATE TABLE tenants (
    id uuid NOT NULL,
    domain text NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    created_at timestamptz NOT NULL,
    updated_at timestamptz NOT NULL
);

CREATE TABLE questions (
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    title text NOT NULL,
    votes int NOT NULL,
    created_at timestamptz NOT NULL,
    updated_at timestamptz NOT NULL
);

ALTER TABLE tenants ADD PRIMARY KEY (id);
ALTER TABLE questions ADD PRIMARY KEY (id, tenant_id);