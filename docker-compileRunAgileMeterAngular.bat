#docker build -f everisapi.API/Dockerfile_Angular -t "agilemeter_angular" .
docker build -t "agilemeter_front" ./front
docker run --name agilemeter_front -p 4200:4200 agilemeter_front