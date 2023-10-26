terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }

}

data "azurerm_resource_group" "rg_name_dev" {
  name = "rg-sco-dev-sea-01"
}

resource "azurerm_storage_account" "asbstorageacc" {
  count                    = var.is_dev ? 1 : 0
  name                     = "asbstorageacc"
  resource_group_name      = data.azurerm_resource_group.rg_name_dev.name
  location                 = data.azurerm_resource_group.rg_name_dev.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = "dev"
  }

}

resource "azurerm_storage_container" "logicAppTrigger" {
  count                 = var.is_dev ? 1 : 0
  name                  = "logicapptrigger"
  storage_account_name  = azurerm_storage_account.asbstorageacc[0].name
  container_access_type = "private"
}

