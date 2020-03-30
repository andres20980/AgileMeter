
FROM golang:1.12-alpine as builder

WORKDIR /go/src/github.com/zerok/jiravars
COPY . .

RUN apk add git && \
    export GO111MODULE=on && \ 
    go mod vendor && \
    CGO_ENABLED=0 go build -ldflags '-s -w' -o /jiravars


FROM alpine:3.10

EXPOSE 9300
# ENTRYPOINT ["/jiravars"]
# jiravars    | time="2019-08-26T08:07:23Z" level=fatal msg="Please specify a config file using --config CONFIG_FILE"
COPY JiravarConfigFile /JiravarConfigFile
CMD /jiravars --config JiravarConfigFile

RUN apk add ca-certificates && addgroup -S app && adduser -S app -G app

USER app

#COPY JiravarConfigFile.yml /JiravarConfigFile.yml
COPY --from=builder /jiravars /