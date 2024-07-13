#!/bin/zsh

# Load environment variables from .env 
source .env

# Run the Docker container with specified environment variables and port mapping
docker run -d \
  --name test-auth \
  -p 5058:80 \
  --network my_custom_network \
  -e ManageUsersLocalConnection=$MANAGE_USERS_LOCAL_CONNECTION \
  -e TokenEndpoint=$TOKEN_ENDPOINT \
  -e ClientId=$CLIENT_ID \
  -e ClientSecret=$CLIENT_SECRET \
  -e UserPoolId=$USER_POOL_ID \
  -e AccessKey=$ACCESS_KEY \
  -e SecretAccessKey=$SECRET_ACCESS_KEY \
  tylersimeone/projectb/manage-auth:latest

if [ $? -ne 0 ]; then
  echo "Docker run command failed!"
  exit 1
fi

echo "Docker container started successfully."