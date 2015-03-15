    var CarrinhoComprasDevmedia = CarrinhoComprasDevmedia || {};
    
    CarrinhoComprasDevmedia.AdminPage = function() {
        var self = this;
        self.Ordems = ko.observableArray([]);
        self.AdminTools = new CarrinhoComprasDevmedia.AdminTools();
    };
    
    CarrinhoComprasDevmedia.AdminTools = function() {
        var self = this;
    
        self.negarPedido = function (item) {
            var SalvarDados = { id: item.id, aprovadaCompra: false };
            aprovacaoInterna(SalvarDados);
        };
        
        self.aprovaPedido = function (item) {
            var SalvarDados = { id: item.id, aprovadaCompra: true };
            aprovacaoInterna(SalvarDados);
        };
        
        var aprovacaoInterna = function(data) {
            $.ajax({
                url: "/api/ordem/" + data.id,
                data: JSON.stringify(data),
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                var match = ko.utils.arrayFirst(viewModel.Ordems(), function(found) {
                    return data.id === found.id;
                });
                match.aprovadaCompra(data.aprovadaCompra);
            }).error(function (data) {
                toastr.error("A sua compra foi inválida!");
            });;
        };
    };
    
    var viewModel;
    $(function () {
        viewModel = new CarrinhoComprasDevmedia.AdminPage();
        hub = $.connection.admin;
    
        ko.applyBindings(viewModel);
    
        hub.client.orderReceived = function (ordem) {
            toastr.info("Uma nova ordem com código = " + ordem.Id + " foi recebida!");
            viewModel.Ordems.push(new CarrinhoComprasDevmedia.Ordem(ordem));
        };
    
        $.connection.hub.start();
    
        $.get("/api/ordem", function (itens) {
            $.each(itens, function (idx, item) {
                viewModel.Ordems.push(new CarrinhoComprasDevmedia.Ordem(item));
            });
        }, "json");
    });