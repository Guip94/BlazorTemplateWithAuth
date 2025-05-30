#!/bin/bash

# Start the script to create the DB and user
/app/configure-db.sh &

# Start SQL Server
/opt/mssql/bin/sqlservr