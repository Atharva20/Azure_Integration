locals {
  tags = {
    environment = var.env
  }

  # Resource Group Name
  resource_group_name = format("rg-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)

  # Storage Account Name
  storage_account_name = format("sa%s%s%s%s", "az", var.env, "sea", var.instance) // saazdevsea01
  storage_account_nametwo = format("satwo%s%s%s%s", "az", var.env, "sea", var.instance) // saazdevsea01

  # FunctionApp Name
  functionapp_name                 = format("fa-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)
  functionapp_storage_account_name = format("fasa%sfa%s%s%s", "az", var.env, "sea", var.instance)

  # Logic Name
  logic_name                 = format("la-%s-%s-%s-%s", "azureintegration", var.env, "sea", var.instance)
  loogic_storage_account_name = format("lasa%sfa%s%s%s", "az", var.env, "sea", var.instance)

}