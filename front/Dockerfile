FROM node:12.16.1
#ENV HTTP_PROXY "http://10.0.8.102:8080"
#ENV HTTPS_PROXY "http://10.0.8.102:8080"
# create & set working directory
RUN mkdir /app
WORKDIR /app
RUN set NODE_OPTIONS=--max-old-space-size=8192

# Copy the HeadCount source code
COPY . .

# Add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

#Get host environment variables
ARG BACKEND_HOST
ARG BACKEND_PORT

#Create environment.prod.ts file
ENV JSON_FMT 'export const environment = { \n\t production:true, \n\t backendHost:"%s", \n\t backendPort: %s \n };'
RUN printf "$JSON_FMT" "$BACKEND_HOST" "$BACKEND_PORT" > src/environments/environment.prod.ts

# Install and cache app dependencies
RUN npm config set registry http://registry.npmjs.org/
RUN npm install -g angular-http-server
RUN npm install

# Prepare the port to be displayed
EXPOSE 4200

# start app (ng server), then prepare the listening port, and host; and then declare the file watching poll period
#CMD ng serve --port 4200 --host 0.0.0.0 --disableHostCheck true
RUN npm run build --prod --max-old-space-size=8192
#CMD ["angular-http-server", "--path wwwroot"]
CMD angular-http-server --path wwwroot -p 4200

