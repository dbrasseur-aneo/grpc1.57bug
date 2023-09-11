#!/usr/bin/env bash

trap "trap - SIGTERM && kill -- -$$" SIGINT SIGTERM EXIT

set -e

dotnet run --framework=net6.0 &
source ./env/bin/activate
sleep 3
python main.py