FROM mysql:latest
ADD dumplatest.sql /docker-entrypoint-initdb.d
ENV MYSQL_ALLOW_EMPTY_PASSWORD 'true'
ENV MYSQL_USER 'root'
ENV MYSQL_DATABASE 'agilemeter'
COPY my.cnf /etc/mysql/my.cnf
