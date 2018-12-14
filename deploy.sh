#!/bin/bash
$(aws ecr get-login --no-include-email --region us-east-1)
docker build -t readysetgo .
docker tag readysetgo:latest 540629508292.dkr.ecr.us-east-1.amazonaws.com/readysetgo:latest
docker push 540629508292.dkr.ecr.us-east-1.amazonaws.com/readysetgo:latest

apt-get install jq -y

TASK=$(aws ecs describe-task-definition --task-definition readysetgo --output json | jq '.taskDefinition.containerDefinitions')
aws ecs register-task-definition --family readysetgo --container-definitions "$TASK" --task-role-arn arn:aws:iam::540629508292:role/ecsTaskExecutionRole

REVISION=$(aws ecs describe-task-definition --task-definition readysetgo --output json  | jq -r '.taskDefinition.revision')
aws ecs update-service --cluster default-ec2 --service readysetgo --task-definition readysetgo:$REVISION

#run chmod +x deploy.sh first if you have permissions issues
