# Use image of Jenkins long time support 
FROM jenkins/jenkins:lts
ARG JENKINS_HOME=/var/jenkins_home

# Change to user root for execute the following sentences
USER root

# Update all the package of the system and it`s install a package for compatibility between DOS and Unix
RUN apt-get update
RUN apt-get install dos2unix

# Copy the credentials for to use the secret key
RUN mkdir -p /root/.ssh
COPY /configs/credentials/.ssh /root/.ssh/
# Is necesary change the permision for work clone git
RUN chmod 644 /root/.ssh/id_rsa.pub
RUN chmod 644 /root/.ssh/known_hosts
RUN chmod 600 /root/.ssh/id_rsa

#RUN mkdir ${JENKINS_HOME}/.ssh
#COPY /configs/credentials/.ssh/ ${JENKINS_HOME}/.ssh/
#RUN chmod 644 ${JENKINS_HOME}/.ssh/id_rsa.pub
#RUN chmod 644 ${JENKINS_HOME}/.ssh/known_hosts
#RUN chmod 600 ${JENKINS_HOME}/.ssh/id_rsa
#RUN chown -R jenkins:jenkins ${JENKINS_HOME}/.ssh/


####################################################  CONFIG JENKINS ####################################################

# Generate the user credentials with plugin Credentials Plugin
COPY /configs/credentials/credentials.xml ${JENKINS_HOME}/credentials.xml
RUN chown jenkins:jenkins -R ${JENKINS_HOME}/credentials.xml

# Change to user jenkins for execute the following sentences
USER jenkins

# Install the plugins necesary 
COPY /configs/plugins.txt /usr/share/jenkins/ref/plugins.txt
RUN /usr/local/bin/install-plugins.sh < /usr/share/jenkins/ref/plugins.txt
# Add the admin user  
COPY /volumes/home/ ${JENKINS_HOME}/
# Add the main config file to the jenkins path  
COPY /configs/jenkins_home_config.xml /usr/share/jenkins/ref/config.xml.override


####################################################  CONFIG .NET ####################################################
# Change to user root for execute the following sentences
USER root

# Install docker
RUN printf "deb http://deb.debian.org/debian stretch main\ndeb http://security.debian.org/debian-security stretch/updates main\ndeb http://deb.debian.org/debian stretch-updates main\ndeb http://ftp.us.debian.org/debian stretch main contrib non-free" > /etc/apt/sources.list

RUN apt-get update

RUN apt-get install apt-transport-https ca-certificates curl gnupg2 software-properties-common --assume-yes

RUN curl -fsSL https://download.docker.com/linux/$(. /etc/os-release; echo "$ID")/gpg | apt-key add -

RUN add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/$(. /etc/os-release; echo "$ID") $(lsb_release -cs) stable"
RUN echo "deb [arch=amd64] https://download.docker.com/linux/$(. /etc/os-release; echo "$ID") $(lsb_release -cs) stable"
RUN apt-get update

RUN apt-get install docker-ce --assume-yes

# Install docker-compose
RUN curl -L "https://github.com/docker/compose/releases/download/1.24.1/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
RUN chmod +x /usr/local/bin/docker-compose

#Install Azure cli
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash
RUN curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | tee /etc/apt/trusted.gpg.d/microsoft.asc.gpg > /dev/null
RUN AZ_REPO=$(lsb_release -cs)
RUN echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/azure-cli.list
RUN apt-get update
RUN apt-get install azure-cli

# Install .NET CLI dependencies
RUN apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.300

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='1D660A323180DF3DA8C6E0EA3F439D6BBEC29670D498AC884F38BF3CDFFBB041C7AFFF66171CDFD24C82394B845B135B057404DEF1FCE9F206853726382BC42B' \
    && sha512sum dotnet.tar.gz \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Install SonarScanner
RUN dotnet tool install --global dotnet-sonarscanner

# Specific path tools for work fine the command
RUN export PATH="$PATH:~/.dotnet/tools"

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # Specific path for tools of .NET
    PATH="${PATH}:/root/.dotnet/tools"
