resource "azurerm_windows_function_app" "azureintegration" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.functionapp_name
  resource_group_name = azurerm_resource_group.resourcegrp[0].name
  location            = azurerm_resource_group.resourcegrp[0].location
  tags                = local.tags

  storage_account_name       = azurerm_storage_account.functionapp_storage_account[0].name
  storage_account_access_key = azurerm_storage_account.functionapp_storage_account[0].primary_access_key
  service_plan_id            = azurerm_service_plan.fa-asp[0].id

  identity {
    type = "SystemAssigned"
  }

  site_config {
  }

  app_settings = {
    "client_storage_account_url"          = local.client_storage_account_url
    "servicebus_fullyqualified_namespace" = local.servicebus_fullyqualified_namespace
    # "AzureWebJobs.PullMsgFromTargetBlobStorage.Disabled" = true
  }
}

#   newtwork_rules
#   {
#     default_action = "Deny"
#     bypass = ["None"]
#     virtual_netowrk_subnet_ids = []
#     ip_rules = []
#   }

# resource "azurerm_private_endpoint" "functionapp_storage_blob_private_endpoint"{
#    count               = var.deploy_tier ? 1 : 0
#    name = ""
#    location = ""
#    resource_group_name = ""
#    subnet_id = ""

#    private_service_connection{
#     name = "" #funcapp name
#     private_connection_resource_id = azurerm_storage_account.name.id
#     subresource_names = ["blob"]
#     is_maual_connection = false
#    }
# }

#####################################################
