# Using Azure Files for AKS Data Volumes

## Tag and Push the Docker Image to ACR
We need to tag the downloaded/built docker image and push the newly tagged image to ACR to be used in the hands-on session. We will use the same ACR instance we created in the Security for AKS session. Use the following commands

```powershell
# Tag the docker image
docker tag "aks-data-volumes-demo" "<your-acr-name>.azurecr.io/aks-data-volumes-demo"

# Login to Azure CLI
az login --use-device-code

# Login to ACR
az acr login --name "<registry-name>"

# Push the image to ACR
docker push "<your-acr-name>.azurecr.io/aks-data-volumes-demo"
```

## Login to AKS and Download Credentials for kubectl

```powershell
# Merge AKS cluster config with your local kube config
az aks get-credentials --name "<aks-name>" --resource-group "aks-security-rg" --admin
```


# Create Persistent Volume using Statically Provisioned Azure File Share
In this example we will be creating a volume using Azure Files and share it across multiple pods.

## 1. Create the Azure Storage Account
```powershell
az storage account create --name "<storage-account-name>" --resource-group "aks-security-rg" --location "southeastasia" --sku "Standard_LRS"
```

## 2. Get the Connection String for the Storage Account
```powershell
$storageConnection = az storage account show-connection-string --name "<storage-account-name>" --resource-group "aks-security-rg" --output tsv
```

## 3. Create the File Share in the Storage Account
```powershell
az storage share create --name "aks-file-share" --connection-string $storageConnection
```

## 4. Get the Storage Account Key
```powershell
$storageKey = az storage account keys list --account-name "<storage-account-name>" --resource-group "aks-security-rg" --query "[0].value" --output tsv
```

## 5. Create the Kubernetes Secret to Access Azure File Share
```powershell
kubectl create secret generic aks-share-secret --from-literal azurestorageaccountname="<storage-account-name>" --from-literal azurestorageaccountkey=$storageKey
```

## 6. Run the Kubetnetes Deployment
```powershell
kubectl create --filename .\deploy-files-static.yml
```


