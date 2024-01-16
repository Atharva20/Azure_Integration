terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
  # backend "azurerm" {
  #   resource_group_name  = "rgazdevsea01"
  #   storage_account_name = "saazdevsea01"
  #   container_name       = "tfstateazdevsea01"
  #   key                  = "terraform.tfstate"
  # }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resourcegrp" {
  count    = var.deploy_tier ? 1 : 0
  name     = local.resource_group_name
  location = var.location
}
