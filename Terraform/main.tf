terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }

}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg_dev" {
  count    = var.deploy_tier ? 1 : 0
  name     = local.resource_group_name
  location = var.location
}

