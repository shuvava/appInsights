#!/bin/sh
./bombardier-windows-amd64.exe -c 100 -n 15000 -t 2s http://localhost:5000/api/errors/HttpClientNoUsingWithExceptions