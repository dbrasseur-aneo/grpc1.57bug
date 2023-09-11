#!/usr/bin/env bash

trap "trap - SIGTERM && kill -- -$$" SIGINT SIGTERM EXIT

set -e

source ./env/bin/activate
python service.py &
sleep 2
python main.py