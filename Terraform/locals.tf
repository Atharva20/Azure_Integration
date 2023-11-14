locals {
  tags = {
    environment = var.env
  }

  # Resource Group Name
  resource_group_name = format("rg-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)

  # Storage Account Name
  storage_account_name = format("sa%s%s%s%s", "az", var.env, "sea", var.instance) // saazdevsea01

  # FunctionApp Name
  functionapp_name                 = format("fa-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)
  functionapp_storage_account_name = format("sa%sfa%s%s%s", "az", var.env, "sea", var.instance)

}