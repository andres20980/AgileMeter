@if  "%~1"=="" (goto emptyParameter) else goto delete

:delete
@echo Erasing %1 project....

oc delete route %1-frontend
oc delete route %1-backend

oc delete svc agilemeterfront
oc delete svc agilemeter
oc delete svc agiledatabase

oc delete dc %1-frontend
oc delete dc %1-backend
oc delete dc %1-mysql

oc delete bc %1-frontend
oc delete bc %1-backend
oc delete bc %1-mysql

oc delete is %1-frontend
oc delete is %1-backend
oc delete is %1-mysql

oc delete pvc %1-mysql

oc delete cm %1-mysql-config

@goto end

:emptyParameter
@echo ERROR:::: parameter APPLICATION_NAME has not been declared.

:end
