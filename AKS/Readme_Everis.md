## AZURE LOGIN 
set http_proxy=
set https_proxy=
az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

## CREATE RESOURCE GROUP AND KUBERNETES CLUSTER
az group create --name AgileMeterRG --location westeurope
az aks create --resource-group AgileMeterRG --name AgileMeterCluster --enable-addons http_application_routing,monitoring --disable-rbac --kubernetes-version 1.12.8 --node-count 2 --generate-ssh-keys --location westeurope
az aks get-credentials --resource-group AgileMeterRG --name AgileMeterCluster

## (TBC) Add Yes Yes to overwrite

## SHOW RESOUCE GROUP DNS ZONE
az aks show --resource-group AgileMeterRG --name AgileMeterCluster --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o table

#################  REPLACE DNS ZONE NAME ON FRONT AND BACK YAML FILES WITH <SERVICENAME.DNS>  #################
#################  REPLACE DNS ZONE NAME ON AGILEMETERFRONT> BUILD>ARGS> BACKEND_HOST AND SET BACKEND_PORT=80 on docker-compose-AKS.yml FILE FOR BACKEND ENTRY  #################


## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
az acr create --resource-group AgileMeterRG --name amapifront --sku Basic --admin-enabled=true
az acr create --resource-group AgileMeterRG --name amapiback --sku Basic --admin-enabled=true
az acr create --resource-group AgileMeterRG --name amapidatabase --sku Basic --admin-enabled=true

az acr credential show -g AgileMeterRG -n amapifront -o table
az acr credential show -g AgileMeterRG -n amapiback -o table
az acr credential show -g AgileMeterRG -n amapidatabase -o table



#################  REPLACE THE CREDENTIALS FOR EACH ENTRY  #################

## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login amapidatabase.azurecr.io -u amapidatabase -p aIs3nUrQ2umByxkuZ028k=FhSSbp1=Sq
docker login amapifront.azurecr.io -u amapifront -p cUX2Es1hRPD4hYneIk+xq99o37Uw7rEU
docker login amapiback.azurecr.io -u amapiback -p y2lmEnBuCR/MA2GI/TeWpPmrRixhGkl+

#################  GENERATE YOUR LOCAL IMAGES DOCKER USING DOCKER-COMPOSE FILE  #################
## CLONE GIT REPOSITORY DEVELOPMENT BRANCH
##git clone --branch Development https://steps.everis.com/git/INNOVASEV/ScrumMeter.git -v  

#################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
## BUILD DOCKER LOCAL IMAGES
cd ScrumMeter
docker-compose -f docker-compose-AKS.yml build
docker images

## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push amapidatabase.azurecr.io/agiledatabase_dev:latest
docker push amapifront.azurecr.io/agilemeterfront_dev:latest
docker push amapiback.azurecr.io/agilemeter_dev:latest

## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
az acr login --name amapidatabase -u amapidatabase -p aIs3nUrQ2umByxkuZ028k=FhSSbp1=Sq
az acr login --name amapifront -u amapifront -p cUX2Es1hRPD4hYneIk+xq99o37Uw7rEU
az acr login --name amapiback -u amapiback -p y2lmEnBuCR/MA2GI/TeWpPmrRixhGkl+

az acr repository list --name amapidatabase --output table
az acr repository list --name amapifront --output table
az acr repository list --name amapiback --output table


## AKS CLUSTER ROLE BINDING AND SERVICE ACCOUNT CREATION KUBE-SYSTEM
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/master/aio/deploy/recommended/kubernetes-dashboard.yaml
kubectl create clusterrolebinding kubernetes-dashboard -n kube-system --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard

########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry amapifront-regsecret --docker-server=amapifront.azurecr.io --docker-username=amapifront --docker-password=cUX2Es1hRPD4hYneIk+xq99o37Uw7rEU --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapiback-regsecret --docker-server=amapiback.azurecr.io --docker-username=amapiback --docker-password=y2lmEnBuCR/MA2GI/TeWpPmrRixhGkl+ --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapidatabase-regsecret --docker-server=amapidatabase.azurecr.io --docker-username=amapidatabase --docker-password=aIs3nUrQ2umByxkuZ028k=FhSSbp1=Sq --docker-email=alberto.muriel.devops@hotmail.com

## DEPLOY YAML FILES TO AKS
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agiledatabase.yaml" --validate=false
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agilemeter.yaml" --validate=false
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agilemeterfront.yaml" --validate=false

kubectl get all

## STOP LOCAL DOCKER SERVICE
docker-machine stop

##BROWSE AKS KUBERNETES DASHBOARDBrowse
az aks browse --resource-group AgileMeterRG --name AgileMeterCluster --subscription "Evaluación gratuita" --listen-port 8010

#################  OPEN FRONTEND <SERVICENAME.DNS> IN  WEB BROWSER  #################  
#################  SEE THE MAGIC  #################  

TIMEOUT /T 20

