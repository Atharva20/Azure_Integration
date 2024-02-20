data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "azintkeyvault" {
  count                       = var.deploy_tier ? 1 : 0
  name                        = "atharvakeyvault"
  location                    = azurerm_resource_group.resourcegrp[0].location
  resource_group_name         = azurerm_resource_group.resourcegrp[0].name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions      = ["Get", "List", "Set"]
    key_permissions         = ["Get", "List"]
    certificate_permissions = ["Get", "List", "Create", "Import"]
  }
}

resource "azurerm_key_vault_secret" "storageconnectionstring" {
  count        = var.deploy_tier ? 1 : 0
  name         = "storageconnectionstring"
  value        = azurerm_storage_account.clientstorageaccount[0].primary_connection_string
  key_vault_id = azurerm_key_vault.azintkeyvault[0].id
}
