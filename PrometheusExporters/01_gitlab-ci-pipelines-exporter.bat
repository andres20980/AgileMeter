@ECHO OFF
REM ## PROXY
set http_proxy=
set https_proxy=

git clone https://github.com/mvisonneau/gitlab-ci-pipelines-exporter.git
cd gitlab-ci-pipelines-exporter/charts
helm package gitlab-ci-pipelines-exporter
helm upgrade -i gitlab-ci-pipelines-exporter ./gitlab-ci-pipelines-exporter-0.0.0.tgz -f gitlab-ci-pipelines-exporter_values.yml

kubectl get all --namespace=monitoring 

TIMEOUT /T 20


