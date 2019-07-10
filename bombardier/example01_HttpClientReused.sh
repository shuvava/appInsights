#!/bin/sh
./bombardier-windows-amd64.exe  -c 40 -n 200 -t 2s http://localhost:5000/api/errors/ReusedHttpClient