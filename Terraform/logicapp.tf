resource "azurerm_logic_app_standard" "testlogicapp" {
  count                      = var.deploy_tier ? 1 : 0
  name                       = local.logic_name
  resource_group_name        = azurerm_resource_group.resourcegrp[0].name
  location                   = azurerm_resource_group.resourcegrp[0].location
  app_service_plan_id        = azurerm_service_plan.la-asp[0].id
  storage_account_name       = azurerm_storage_account.logicapp_storage_account[0].name
  storage_account_access_key = azurerm_storage_account.logicapp_storage_account[0].primary_access_key

  app_settings = {
    "FUNCTIONS_WORKER_RUNTIME"            = "node"
    "FUNCTIONS_EXTENSION_VERSION"         = "~4"
    "WEBSITE_NODE_DEFAULT_VERSION"        = "~18"
    "WEBSITE_RUN_FROM_PACKAGE"            = "1"
    "client_storage_account_url"          = local.client_storage_account_url
    "servicebus_fullyqualified_namespace" = local.servicebus_fullyqualified_namespace
  }

  identity {
    type = "SystemAssigned"
  }
}
