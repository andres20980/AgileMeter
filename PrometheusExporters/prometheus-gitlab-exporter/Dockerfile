FROM debian:stretch-slim
#ENV HTTP_PROXY "http://10.0.8.102:8080"
#ENV HTTPS_PROXY "http://10.0.8.102:8080"
# create & set working directory

RUN mkdir /app
WORKDIR /app
#RUN set NODE_OPTIONS=--max-old-space-size=6096

# Copy the HeadCount source code
COPY . .

# Add `/app/node_modules/.bin` to $PATH
#ENV PATH /app/node_modules/.bin:$PATH

#Get host environment variables
ARG URL
ENV URL ${URL}
ARG TOKEN
ENV TOKEN ${TOKEN}
ARG POLL_INTERVAL
ENV POLL_INTERVAL ${POLL_INTERVAL}
ARG BIND
ENV BIND ${BIND}


# Install and cache app dependencies
RUN apt-get update --allow-unauthenticated && apt-get install -y wget htop 
RUN wget -O prometheus-gitlab-exporter https://github.com/j6s/prometheus-gitlab-exporter/releases/download/1.0.1/gitlab-exporter__linux-amd64
#RUN CURL https://github.com/j6s/prometheus-gitlab-exporter/releases/download/1.0.1/gitlab-exporter__linux-amd64 -o /app/prometheus-gitlab-exporter


# Prepare the port to be displayed
EXPOSE 8123

RUN echo $URL
RUN echo $TOKEN
RUN echo $POLL_INTERVAL
RUN echo $BIND


# Start prometheus-gitlab-exporter service listener
RUN pwd
RUN chmod 755 prometheus-gitlab-exporter
RUN ls -al

#RUN /app/prometheus-gitlab-exporter --url="$URL" --token="$TOKEN" --poll-interval="$POLL_INTERVAL" &
CMD /app/prometheus-gitlab-exporter --url=${URL} --token=${TOKEN} --poll-interval=${POLL_INTERVAL}




