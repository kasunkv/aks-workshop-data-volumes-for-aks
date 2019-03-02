# Data Volumes for AKS - AKS Workshop 2019 Colombo
Data Volumes for AKS Hands-On Session Guidelines for AKS Workshop 2019. Make sure you read the prerequisites section and install all the necessary tools needed to follow along with the hands-on session.

# Content of the Hands-On Session

* [Using Azure Files for AKS Data Volumes](https://github.com/kasunkv/aks-workshop-data-volumes-for-aks/blob/master/guidelines/using-azure-files-for-aks-data-volumes.md)
* [Using Azure Disks for AKS Data Volumes](https://github.com/kasunkv/aks-workshop-data-volumes-for-aks/blob/master/guidelines/using-azure-disks-for-aks-data-volumes.md)

> ### Note:
> Make sure you have read and completed the **Prerequisites** section to install and configure the tools needed to follow along in the hands-on session. The prerequisites can be found below.


# Prerequisites
Make sure to install the following tools in your development machine prior to the hands-on session.

## 1. Install Git
Make sure you have git installed so you can clone the source code for the hands-on session. You can download Git from the [Official Download Page](https://git-scm.com/downloads)

## 2. Latest Version of Azure CLI
The current latest version of the Azure CLI is 2.0.59 as of writing this guidelines. Download and Install the Azure CLI for your operating system by following the links given below.

* [Install On Windows](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)
* [Install on MacOS](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-macos?view=azure-cli-latest)
* [Install on Linux](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

Run the following commands to verify the installation and the CLI version
```powershell
# Check the Azure CLI version
az --version

# Login to Azure Subscription
az login --use-device-code
```

## 3. Kubectl - Kubernetes Command-Line Client
You need to install `kubectl` on your local machine to manage your Kubernetes cluster on AKS. It's really simple. Once you have Azure CLI installed run the following command.

```powershell
# Install kubectl using Azure CLI
az aks install-cli
```

You can run the following command to connect to your AKS cluster by downloading the credentials and configuring kubectl to use them

```powershell
# Download and merge the AKS config with your local kubernetes config
az aks get-credentials --resource-group "<your-resource-group>" --name "<aks-cluster-name>"
```

Once the credentials are downloaded. Run the following command to make sure you have access to the AKS cluster.

```powershell
# You should see the available nodes in your AKS cluster
kubectl get nodes
```

# Building the Sample Application
The hands-on session contains a sample application that will demonstrate writing a text file in to a volume and reading the files inside the volume. You can build the Docker image locally and push it to your Azure Container Register (ACR).

## 1. Build the Docker Image

```powershell
# Build the docker image
docker build -f .\Dockerfile --tag aks-data-volumes-demo .
```

## 2. Run the Application Locally.
Create a folder in you `C:` drive with the name `files`. **e.g.** _C:\files_

### **Option 01:** Running Using Visual Studio
You can run the application using Visual Studio. 

### **Option 02:** Running the Docker Container
Use the following command to run the docker container.
```powershell
# Run the docker container with a volume mounted
docker run -p 5000:80 -v "c:/files:/files" aks-data-volumes-demo
```


# Optional Steps
If you don't want to build the docker image used for the hands-on session. You can download the pre-made docker image from the Docker Hub. Use the following command to download the docker image to your local machine.

```powershell
# Pull the kasunkv/aks-data-volumes-demo image from docker hub
docker pull kasunkv/aks-data-volumes-demo
```

After that you can tag the image for the ACR and push it to Azure

# Using Azure Cloud Shell
If anything goes wrong with your local development environment when you try to use Azure CLI or kubectl commands, you can use the Azure Cloud Shell to execute the same commands and follow along.


# References


### [Kubernetes Volumes](https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/)

### [Kubernetes Persistent Volumes](https://kubernetes.io/docs/concepts/storage/persistent-volumes/)

### [Kubernetes StorageClasses](https://kubernetes.io/docs/concepts/storage/storage-classes/)

### [Kubernetes ReplicaSet](https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/)

### [Kubernetes StatefulSet](https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/#limitations)

### [Kubernetes Storage Concepts](https://docs.microsoft.com/en-us/azure/aks/concepts-storage)