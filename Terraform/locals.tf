locals {
  tags = {
    environment = var.env
  }

  # Resource Group Name
  resource_group_name = format("rg-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)

  # Client Storage Account Name
  storage_account_name       = format("sa%s%s%s%s", "az", var.env, "sea", var.instance)               // saazdevsea01
  storage_account_nametwo    = format("satwo%s%s%s%s", "az", var.env, "sea", var.instance)            // saazdevsea01
  client_storage_account_url = format("https://%s.blob.core.windows.net", local.storage_account_name) // https://saazdevsea01.blob.core.windows.net

  # FunctionApp Name
  functionapp_name                 = format("fa-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance) // fa-azureintegration-devv-sea-01
  functionapp_storage_account_name = format("fasa%sfa%s%s%s", "az", var.env, "sea", var.instance)

  # Logic Name
  logic_name                  = format("la-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)
  logic_storage_account_name = format("lasa%sfa%s%s%s", "az", var.env, "sea", var.instance)

  # Service Bus
  servicebus_namespace                = format("sa-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance) // sa-azureintegration-devv-sea-01
  servicebus_fullyqualified_namespace = format("%s.servicebus.windows.net", local.servicebus_namespace)            // sa-azureintegration-devv-sea-01.servicebus.windows.net

  # AppServicePlan
  func_appservice_name  = format("fa-asp-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)
  logic_appservice_name = format("la-asp-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)

}