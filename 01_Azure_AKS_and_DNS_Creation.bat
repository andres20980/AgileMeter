@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=
CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

REM ## CREATE RESOURCE GROUP AND KUBERNETES CLUSTER
CALL az group create --name AgileMeterRG --location westeurope
CALL az aks create --resource-group AgileMeterRG --name AgileMeterCluster --enable-addons http_application_routing,monitoring --disable-rbac --kubernetes-version 1.12.8 --node-count 2 --generate-ssh-keys --location westeurope
CALL az aks get-credentials --resource-group AgileMeterRG --name AgileMeterCluster

REM ## (TBC) Add Yes Yes to overwrite

REM ## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
CALL az acr create --resource-group AgileMeterRG --name amapifront --sku Basic --admin-enabled=true
CALL az acr create --resource-group AgileMeterRG --name amapiback --sku Basic --admin-enabled=true
CALL az acr create --resource-group AgileMeterRG --name amapidatabase --sku Basic --admin-enabled=true

CALL az acr credential show -g AgileMeterRG -n amapifront --query passwords[0].value
CALL az acr credential show -g AgileMeterRG -n amapiback --query passwords[0].value
CALL az acr credential show -g AgileMeterRG -n amapidatabase --query passwords[0].value

REM ## SHOW RESOUCE GROUP DNS ZONE
CALL az aks show --resource-group AgileMeterRG --name AgileMeterCluster --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o table


REM #################  REPLACE THE CREDENTIALS FOR EACH ENTRY  #################
REM ###########  REPLACE DNS ZONE NAME ON AGILEMETERFRONT> BUILD>ARGS> BACKEND_HOST        #################
REM ###########  AND SET BACKEND_PORT=80 on docker-compose-AKS.yml FILE FOR BACKEND ENTRY  #################


TIMEOUT /T 20