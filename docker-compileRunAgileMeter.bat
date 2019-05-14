docker build -t "agilemeter" ./everisapi.API
docker run --name agilemeter --link autodumperagile -p 60406:60406 agilemeter