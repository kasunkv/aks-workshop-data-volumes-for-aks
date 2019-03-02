# Using Azure Disks for AKS Data Volumes

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

# Create Persistent Volume using Statically Provisioned Azure Disk

## 1. Get a Reference to the Node Resource Group of the AKS
When we create the AKS cluster it will automatically create a different resource group to put all the cluster related resources (e.g. Networking resources, Virtual Machines, Availability Sets etc.). The AKS service principle has Contributor access to this Resource Group. Therefore we can put our Azure Disk in this Resource Group to automatically give AKS access to the disk.

```powershell
$nodeRG = az aks show --name "<aks-name>" --resource-group "aks-security-rg" --query "nodeResourceGroup" --output tsv
```

## 2. Create the Azure Disk and Get a Reference to the Resource ID
```powershell
$diskId = az disk create --resource-group $nodeRG --name "<disk-name>" --size-gb 20 --query "id" --output tsv
```

## 3. Update the Deployment Manifest
```yaml
spec:
containers:
- name: data-vol-disk-static
  image: <your-acr-name>.azurecr.io/aks-data-volumes-demo
  imagePullPolicy: IfNotPresent
  volumeMounts:
    - name: files
    mountPath: /files
volumes:
- name: files
  azureDisk:
    kind: Managed
    diskName: akssecuritydisk01
    diskURI: <azure-disk-id>
```

## 4. Run the Kubetnetes Deployment
```powershell
kubectl create --filename .\deploy-disk-static.yml
```

# Create Persistent Volume using Dynamically Provisioned Azure Disk
To dynamically generate a persistent volume using Azure Disks we need to use a Kubernetes StorageClass. AKS includes two pre-created StorageClasses which are configured to use Azure Disks

* _**default**_:  Standard storage created using HDDs
* _**managed-premium**_: Premium storage created using Premium SSDs.

```powershell
# to see the available storage classes use the following command
kubectl get sc
```

## 1. Create a PersistentVolumeClaim using Pre-Created Storage Class
We can include the following PVC definition in the deployment YAML file to create the PersistentVolumeClaim.

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: az-managed-premium-disk
spec:
  accessModes:
  - ReadWriteOnce
  storageClassName: managed-premium
  resources:
    requests:
      storage: 2Gi
```

## 2. Update the Kubenetes Deployment Manifest file with the PVC
```yaml
spec:
  containers:
  - name: data-vol-disk-dynamic
    image: <your-acr-name>.azurecr.io/aks-data-volumes-demo
    imagePullPolicy: IfNotPresent
    volumeMounts:
    - name: files
      mountPath: /files
  volumes:
  - name: files
    persistentVolumeClaim:
      claimName: az-managed-premium-disk
```

## 3. Run the Kubernetes Deployment
```powershell
kubectl create --filename .\deploy-disk-dynamic.yml
```