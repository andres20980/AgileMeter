## AZURE LOGIN 
set http_proxy=
set https_proxy=
az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

## CREATE RESOURCE GROUP AND KUBERNETES CLUSTER
az group create --name AgileMeterRG --location francecentral
az aks create --resource-group AgileMeterRG --name AgileMeterCluster --enable-addons monitoring --kubernetes-version 1.12.8 --generate-ssh-keys --location francecentral

## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
az acr create --resource-group AgileMeterRG --name amapifront --sku Basic --admin-enabled=true
az acr create --resource-group AgileMeterRG --name amapiback --sku Basic --admin-enabled=true
az acr create --resource-group AgileMeterRG --name amapidatabase --sku Basic --admin-enabled=true

az acr credential show -g AgileMeterRG -n amapifront
az acr credential show -g AgileMeterRG -n amapiback
az acr credential show -g AgileMeterRG -n amapidatabase

#################  REPLACE THE CREDENTIALS FOR EACH ENTRY  #################

## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login amapidatabase.azurecr.io -u amapidatabase -p twN=NvwcKMXccHC1gYPBQphAR0l62RtY
docker login amapifront.azurecr.io -u amapifront -p Och0fEz/0mFFmhCRQWAgPihvqWm3PfS3
docker login amapiback.azurecr.io -u amapiback -p mrzg3koI9/h4jdvrU4EBkvZLu0mANK5l

#################  GENERATE YOUR LOCAL IMAGES DOCKER USING DOCKER-COMPOSE FILE  #################
## CLONE GIT REPOSITORY DEVELOPMENT BRANCH
git clone --branch Development https://steps.everis.com/git/INNOVASEV/ScrumMeter.git -v  

#################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
## BUILD DOCKER LOCAL IMAGES
cd ScrumMeter
docker-compose -f docker-compose-AKS.yml --build
docker images

## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push amapidatabase.azurecr.io/agiledatabase_dev:latest
docker push amapifront.azurecr.io/agilemeterfront_dev:latest
docker push amapiback.azurecr.io/agilemeter_dev:latest

## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
az acr login --name amapidatabase -u amapidatabase -p twN=NvwcKMXccHC1gYPBQphAR0l62RtY
az acr login --name amapifront -u amapifront -p Och0fEz/0mFFmhCRQWAgPihvqWm3PfS3
az acr login --name amapiback -u amapiback -p mrzg3koI9/h4jdvrU4EBkvZLu0mANK5l

az acr repository list --name amapidatabase --output table
az acr repository list --name amapifront --output table
az acr repository list --name amapiback --output table


########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry amapifront-regsecret --docker-server=amapifront.azurecr.io --docker-username=amapifront --docker-password=zZpndR1T1ZRoe=UwOg5aIiGSug9TS/CB --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapiback-regsecret --docker-server=amapiback.azurecr.io --docker-username=amapiback --docker-password=4Xyy2uQT5vNP2B2lWWmtHDeKdnnhWZc= --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapidatabase-regsecret --docker-server=amapidatabase.azurecr.io --docker-username=amapidatabase --docker-password=20WxT4a3CPeE99EqfZ6N6qOYBQPsg=dr --docker-email=alberto.muriel.devops@hotmail.com

## CREATE DNS IN AZURE (POSSIBLY CREATED ON STEP -CREATE RESOURCE GROUP AND KUBERNETES CLUSTER)


#################  EDIT INGRESS SPEC>RULES>HOST ENTRY IN EACH YAML FILE WITH <SERVICENAME.DNS> #################

## DEPLOY YAML FILES TO AKS
kubectl apply -f docker-compose-AKS\templates\agiledatabase.yaml --validate=false
kubectl apply -f docker-compose-AKS\templates\agilemeter.yaml --validate=false
kubectl apply -f docker-compose-AKS\templates\agilemeterfront.yaml --validate=false


#################  OPEN FRONTENDS <SERVICENAME.DNS> INA  WEB BROWSER  #################  
#################  SEE THE MAGIC  #################  

TIMEOUT /T 20

