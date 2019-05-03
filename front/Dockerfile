FROM node:9.6.1
#ENV HTTP_PROXY "http://10.0.8.102:8080"
#ENV HTTPS_PROXY "http://10.0.8.102:8080"
# create & set working directory
RUN mkdir /app
WORKDIR /app

# Copy the HeadCount source code
COPY . .

# Add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH
# Install and cache app dependencies
RUN npm config set registry http://registry.npmjs.org/
RUN npm install

# Prepare the port to be displayed
EXPOSE 4200

# start app (ng server), then prepare the listening port, and host; and then declare the file watching poll period
CMD ng serve --port 4200 --host 0.0.0.0 --disableHostCheck true
