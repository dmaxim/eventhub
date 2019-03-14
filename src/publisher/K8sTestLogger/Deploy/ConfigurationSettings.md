
## Update app settints connection string to point at dev event hub

kubectl create secret generic k8s-logger-config --from-file=./appsettings.json -n development
