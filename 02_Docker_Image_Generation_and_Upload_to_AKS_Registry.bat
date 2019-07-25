@ECHO OFF 
set http_proxy=
set https_proxy=
CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

for /f %%i in ('CALL az acr credential show -g AgileMeterRG -n amapifront --query passwords[0].value') do set amapifrontPassword=%%i
for /f %%i in ('CALL az acr credential show -g AgileMeterRG -n amapiback --query passwords[0].value') do set amapibackPassword=%%i
for /f %%i in ('CALL az acr credential show -g AgileMeterRG -n amapidatabase --query passwords[0].value') do set amapidatabasePassword=%%i

ECHO %amapifrontPassword%
ECHO %amapibackPassword%
ECHO %amapidatabasePassword%

REM ## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login amapidatabase.azurecr.io -u amapidatabase -p %amapidatabasePassword%
docker login amapifront.azurecr.io -u amapifront -p %amapifrontPassword%
docker login amapiback.azurecr.io -u amapiback -p %amapibackPassword%

REM #################  GENERATE YOUR LOCAL IMAGES DOCKER USING DOCKER-COMPOSE FILE  #################
REM ## CLONE GIT REPOSITORY DEVELOPMENT BRANCH
REM ##git clone --branch Development https://steps.everis.com/git/INNOVASEV/ScrumMeter.git -v  

REM #################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
REM ## BUILD DOCKER LOCAL IMAGES
cd ScrumMeter
docker-compose -f docker-compose-AKS.yml build
docker images

REM ## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push amapidatabase.azurecr.io/agiledatabase_dev:latest
docker push amapifront.azurecr.io/agilemeterfront_dev:latest
docker push amapiback.azurecr.io/agilemeter_dev:latest

REM ## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
CALL az acr login --name amapidatabase -u amapidatabase -p %amapidatabasePassword%
CALL az acr login --name amapifront -u amapifront -p %amapifrontPassword%
CALL az acr login --name amapiback -u amapiback -p %amapibackPassword%

CALL az acr repository list --name amapidatabase 
CALL az acr repository list --name amapifront
CALL az acr repository list --name amapiback 


REM ## AKS CLUSTER ROLE BINDING AND SERVICE ACCOUNT CREATION KUBE-SYSTEM
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/master/aio/deploy/recommended/kubernetes-dashboard.yaml
kubectl create clusterrolebinding kubernetes-dashboard -n kube-system --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard

REM ########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry amapifront-regsecret --docker-server=amapifront.azurecr.io --docker-username=amapifront --docker-password=%amapifrontPassword% --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapiback-regsecret --docker-server=amapiback.azurecr.io --docker-username=amapiback --docker-password=%amapibackPassword% --docker-email=alberto.muriel.devops@hotmail.com
kubectl create secret docker-registry amapidatabase-regsecret --docker-server=amapidatabase.azurecr.io --docker-username=amapidatabase --docker-password=%amapidatabasePassword% --docker-email=alberto.muriel.devops@hotmail.com

REM ## DEPLOY YAML FILES TO AKS
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agiledatabase.yaml" --validate=false
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agilemeter.yaml" --validate=false
kubectl apply -f "docker-compose-AKS\templates\AgileMeter Unified Yamls\agilemeterfront.yaml" --validate=false

kubectl get all

REM ## SHOW RESOUCE GROUP DNS ZONE
CALL az aks show --resource-group AgileMeterRG --name AgileMeterCluster --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o table

REM ##BROWSE AKS KUBERNETES DASHBOARDBrowse
CALL az aks browse --resource-group AgileMeterRG --name AgileMeterCluster --listen-port 8010

REM #################  OPEN FRONTEND <SERVICENAME.DNS> IN  WEB BROWSER  #################  
REM #################  SEE THE MAGIC  #################  

TIMEOUT /T 20