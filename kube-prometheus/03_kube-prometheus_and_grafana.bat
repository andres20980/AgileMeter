@ECHO OFF 

REM ## PROXY
set http_proxy=
set https_proxy=


REM ## PRERREQUISITES
REM # SOURCE DOCUMENTATION
REM # https://samcogan.com/use-azure-monitor-metrics-in-kubernetes-with-prometheus-and-promitor/
REM # https://itnext.io/using-prometheus-in-azure-kubernetes-service-aks-ae22cada8dd9

REM # INSTALL HELM AND ADD COREOS REPOSITORY
REM # INSTALL HELM FROM https://github.com/helm/helm
REM # I USED CHOCOLATEY ---> choco install kubernetes-helm
helm init
helm repo add coreos https://s3-eu-west-1.amazonaws.com/coreos-charts/stable/

REM # Install Prometheus operator and kube-prometheus
helm install coreos/prometheus-operator --name prometheus-operator --namespace monitoring --set rbacEnable=false
helm install coreos/kube-prometheus --name kube-prometheus --namespace monitoring --set global.rbacEnable=false 

kubectl create namespace monitoring

REM # SHOW PROMETHEUS MONITOR AND SERVICES RESOURCES
kubectl get prometheus --all-namespaces -l release=kube-prometheus
kubectl get servicemonitor --all-namespaces -l release=kube-prometheus
kubectl get service --all-namespaces -l release=kube-prometheus -o=custom-columns=NAMESPACE:.metadata.namespace,NAME:.metadata.name

REM # EXPORTED NODE -> SHOWS A PODS SET CALLED
kubectl get service --all-namespaces -l component=node-exporter -o=custom-columns=NAMESPACE:.metadata.namespace,NAME:.metadata.name

REM ## DOWNLOAD MANUALLY BASE64 FROM --> https://www.proxoft.com/base64/downloads/base64.exe
REM ######## GET GRAFANA CREDENTIALS  --->>  admin/admin #########
REM ## WIP #INVESTIGATE# echo username:$(kubectl get secret --namespace monitoring kube-prometheus-grafana -o jsonpath="{.data.user}"|base64 --decode;echo)
REM ## WIP #INVESTIGATE# echo password:$(kubectl get secret --namespace monitoring kube-prometheus-grafana -o jsonpath="{.data.password}"|base64 --decode;echo)


REM ## PORT 3000 FOWARD TO GRAFANA POD
REM ## WIP #INVESTIGATE# for /f %%i in ('CALL kubectl get pod --namespace monitoring -l app=kube-prometheus-grafana -o template --template "{{(index .items 0).metadata.name}}') do set prometheus-pod=%%i
REM ## WIP #INVESTIGATE# kubectl --namespace monitoring port-forward $prometheus-pod 3000:3000
kubectl get pod --namespace monitoring -l app=kube-prometheus-grafana -o template --template "{{(index .items 0).metadata.name}}
REM ## WIP #CREATE SERVICE AND INGRESS YAML# ----> kubectl --namespace monitoring port-forward $prometheus-pod 3000:3000

REM # CREATE SERVICE AND INGRESS TO PUBLISH GRAFANA
kubectl apply -f kube-prometheus-ingress.yaml


TIMEOUT /T 20
