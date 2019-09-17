@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=

REM ## Logging in through command line is not supported. Is required to use 'az login' to authenticate through browser.
REM CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!
REM ## CALL az login -u bjabinn1978_2@hotmail.com -p 

REM ## Delete context and cluster in Kube config local file
CALL kubectl config delete-context AgileMeterClusterResource
CALL kubectl config delete-cluster AgileMeterClusterResource

REM ## CREATE RESOURCE GROUP AND KUBERNETES CLUSTER
CALL az group create --name AgileMeterResourceGroup --location westeurope
CALL az aks create --resource-group AgileMeterResourceGroup --name AgileMeterClusterResource --enable-addons http_application_routing,monitoring --disable-rbac --kubernetes-version 1.12.8 --node-count 2 --generate-ssh-keys --location westeurope
CALL az aks get-credentials --resource-group AgileMeterResourceGroup --name AgileMeterClusterResource

REM ## (TBC) Add Yes Yes to overwrite

REM ## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
CALL az acr create --resource-group AgileMeterResourceGroup --name amapifront --sku Basic --admin-enabled=true
CALL az acr create --resource-group AgileMeterResourceGroup --name amapiback --sku Basic --admin-enabled=true
CALL az acr create --resource-group AgileMeterResourceGroup --name amapidatabase --sku Basic --admin-enabled=true

CALL az acr credential show -g AgileMeterResourceGroup -n amapifront --query passwords[0].value
CALL az acr credential show -g AgileMeterResourceGroup -n amapiback --query passwords[0].value
CALL az acr credential show -g AgileMeterResourceGroup -n amapidatabase --query passwords[0].value

REM ## SHOW RESOUCE GROUP DNS ZONE
CALL az aks show --resource-group AgileMeterResourceGroup --name AgileMeterClusterResource --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o table


REM #################  REPLACE THE CREDENTIALS FOR EACH ENTRY  #################
REM ###########  REPLACE DNS ZONE NAME ON AGILEMETERFRONT> BUILD>ARGS> BACKEND_HOST        #################
REM ###########  AND SET BACKEND_PORT=80 on docker-compose-AKS.yml FILE FOR BACKEND ENTRY  #################


TIMEOUT /T 20