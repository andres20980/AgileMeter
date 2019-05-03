docker build -t "autodumperagile" ./AutoDumperAgile
docker run --name autodumperagile -p 3306:3306 autodumperagile