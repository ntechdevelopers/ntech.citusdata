README.md

This project sets up a Citus Data cluster using Docker Compose, initializes the database schema, and seeds it with data.

## Getting Started

### Prerequisites

* Docker
* Docker Compose

### Setup

1.  **Clone the repository:**

    ```bash
    git clone <repository_url>
    cd ntech.citusdata
    ```

2.  **Start the Citus Data cluster:**

    ```bash
    docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
    ```

3.  **Verify running containers:**

    ```bash
    docker ps
    ```

4.  **Connect to PostgreSQL (inside the master container):**

    ```bash
    docker exec -it postgresql bash
    su postgres
    psql
    ```

    Alternatively, you can connect directly using psql:

    ```bash
    docker exec -it citus_master psql -U postgres
    ```

5.  **Initialize the database schema and seed data:**

    Once connected to psql, run the following scripts:

    ```sql
    -- From psql prompt:
    \i /init.sql
    \i /sharding.sql
    \i /seeding.sql
    ```

    *Note: These files are located in the project root and are copied into the Docker container.*

## Project Structure

*   `docker-compose.yml`: Defines the core Citus Data services.
*   `docker-compose.override.yml`: Overrides or extends the main Docker Compose configuration.
*   `init.sql`: SQL script for initial database schema setup.
*   `sharding.sql`: SQL script for configuring table sharding with Citus.
*   `seeding.sql`: SQL script for populating the database with sample data.
*   `src/`: Contains the .NET Core application (if applicable).

## Host Configuration (for local development)

Add the following entries to your hosts file (e.g., `/etc/hosts` on Linux/macOS or `C:\Windows\System32\drivers\etc\hosts` on Windows):

```
127.0.0.1	ntechdevelopers.local
127.0.0.1	api.ntechdevelopers.local
``` 