﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.blade.isLoading = false;

    $scope.createItem = function () {
        $scope.blade.isLoading = true;

        $scope.blade.item.$updateitem(null,
            function (dbItem) {
                //TODO: need better way to find category list blade.
                var categoryListBlade = $scope.blade.parentBlade;

                if (categoryListBlade.controller != 'virtoCommerce.catalogModule.categoriesItemsListController') {
                    categoryListBlade = categoryListBlade.parentBlade;
                }

                categoryListBlade.refresh();

                var newBlade = {
                    id: "listItemDetail",
                    itemId: dbItem.id,
                    title: dbItem.name,
                    subtitle: 'Item details',
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, categoryListBlade);
            });
    }

    $scope.openBlade = function (type) {
        var newBlade = null;
        switch (type) {
            case 'properties':
                newBlade = {
                    id: "newProductProperties",
                    item: $scope.blade.item,
                    title: $scope.blade.item.name,
                    subtitle: 'item properties',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardPropertiesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-detail.tpl.html'
                };
                break;
            case 'images':
                newBlade = {
                    id: "newProductImages",
                    item: $scope.blade.item,
                    title: $scope.blade.item.name,
                    subtitle: 'item images',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardImagesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-image-detail.tpl.html'
                };
                break;
            case 'seo':
                newBlade = {
                    id: "newProductSeoDetail",
                    item: $scope.blade.item,
                    title: $scope.blade.item.name,
                    subtitle: 'Seo details',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductSeoDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
                };
                break;
            case 'review':
                if ($scope.blade.item.reviews != undefined && $scope.blade.item.reviews.length > 0) {
                    newBlade = {
                        id: "newProductEditorialReviewsList",
                        currentEntities: $scope.blade.item.reviews,
                        title: $scope.blade.item.name,
                        subtitle: 'Product Reviews',
                        controller: 'virtoCommerce.catalogModule.newProductWizardReviewsController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReviews-list.tpl.html'
                    };
                } else {
                    newBlade = {
                        id: 'editorialReviewWizard',
                        currentEntity: { languageCode: getCatalog().defaultLanguage.languageCode },
                        languages: getCatalog().languages,
                        title: 'Review',
                        subtitle: 'Product Review',
                        bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                        controller: 'virtoCommerce.catalogModule.editorialReviewDetailWizardStepController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
                    };
                }
                break;
        }

        if (newBlade != null) {
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        }
    }

    $scope.codeValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.getUnfilledProperties = function () {
        return _.filter($scope.blade.item.properties, function (p) {
            return p != undefined && p.values.length > 0 && p.values[0].value.length > 0;
        });
    }

    function getCatalog() {
        var parentBlade = $scope.blade.parentBlade;
        while (!parentBlade.catalog) {
            parentBlade = parentBlade.parentBlade;
        }
        return parentBlade.catalog;
    }
}]);


