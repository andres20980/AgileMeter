@ECHO OFF 
set http_proxy=
set https_proxy=

kubectl create namespace monitoring

kubectl apply -f dockprom\alertmanager-configmap.yaml
kubectl apply -f dockprom\alertmanager-deployment.yaml
kubectl apply -f dockprom\alertmanager-service.yaml
kubectl apply -f dockprom\caddy-deployment.yaml
kubectl apply -f dockprom\caddy-service.yaml
kubectl apply -f dockprom\cadvisor-deployment.yaml
kubectl apply -f dockprom\cadvisor-service.yaml
kubectl apply -f dockprom\grafana-configmap.yaml
kubectl apply -f dockprom\grafana-deployment.yaml
kubectl apply -f dockprom\grafana-service-ingress.yaml
kubectl apply -f dockprom\nodeexporter-deployment.yaml
kubectl apply -f dockprom\nodeexporter-service-ingress.yaml
kubectl apply -f dockprom\nodeexporter-service.yaml
kubectl apply -f dockprom\prometheus-configmap.yaml
kubectl apply -f dockprom\prometheus-deployment.yaml
kubectl apply -f dockprom\prometheus-service-ingress.yaml
kubectl apply -f dockprom\pushgateway-deployment.yaml
kubectl apply -f dockprom\pushgateway-service.yaml

pause 10

kubectl get all --namespace=monitoring



TIMEOUT /T 20