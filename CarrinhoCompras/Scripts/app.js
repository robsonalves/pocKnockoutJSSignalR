var CarrinhoCompras = CarrinhoCompras || {};
    
    CarrinhoCompras.Page = function() {
        var self = this;
        self.Carrinho = new CarrinhoCompras.Carrinho();
        self.Livros = ko.observableArray([]);
        self.UserId = "";
    };
    
    CarrinhoCompras.Carrinho = function() {
        var self = this;
    
        self.items = ko.observableArray([]);
        self.add = function (item) {
            if (item.quantidadeItensSelecionados() <= item.quantidade && item.quantidadeItensSelecionados() > 0) {
                self.items.remove(function (p) { return p.id === item.id; });
                self.items.push(new CarrinhoCompras.CarrinhoItem(item));
            }
        };
    
        self.remove = function (item) {
            self.items.remove(function(p) { return p.id === item.id; });
        };
    
        self.checkOut = function () {
            var SalvarDados = { NomeCliente: viewModel.UserId, ValorTotal: self.somaTotal() };
            SalvarDados.Livros = [];
            $.each(self.items(), function (idx, item) {
                var dataItem = ko.toJS(item.produto);
                SalvarDados.Livros.push(dataItem);
            });
    
            $.ajax({
                url: "/api/ordem",
                data: JSON.stringify(SalvarDados),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                self.items.removeAll();
                $.each(viewModel.Livros(), function (idx,livro) {
                    console.log(livro);
                    livro.quantidadeItensSelecionados(0);
                });
                toastr.success("Sua compra foi submetida!");
            }).error(function(data) {
                toastr.error("Seu pedido é inválido, por favor, revise antes de finalizar a compra!");
            });
        };
    
        self.somaTotal = ko.computed(function() {
            var total = 0;
            $.each(self.items(), function () { total += this.subtotal(); });
            return total;
        });
    };
    
    CarrinhoCompras.CarrinhoItem = function(livro) {
    var self = this;

    self.id = livro.id;
    self.produto = livro;
    self.quantidade = ko.observable(livro.quantidadeItensSelecionados());
    self.subtotal = ko.computed(function() {
        return self.produto ? self.produto.preco * parseInt("0" + self.quantidade(), 10) : 0;
    });
};
    
    var viewModel;
    $(function () {
        viewModel = new CarrinhoCompras.Page();
        hub = $.connection.Carrinho;
    
        ko.applyBindings(viewModel);
    
        hub.client.updateProdutoCount = function (livro) {
            var match = ko.utils.arrayFirst(viewModel.Livros(), function (item) {
                return livro.Id === item.id;
            });
            toastr.info("O produto de código = "+ livro.Id +" teve seu estoque alterado!");
            match.quantidade(livro.quantidade);
        };
        
        hub.client.orderAprovado = function (_aprovadaCompra) {
            if (_aprovadaCompra.Aprovado) {
                toastr.success("Seu pedido de código = " + _aprovadaCompra.id + " foi aprovado!");
            } else {
                toastr.error("Seu pedido de código = " + _aprovadaCompra.id + " foi rejeitado!");
            }
        };
    
        $.connection.hub.start().done(function() {
            viewModel.UserId = $.connection.hub.id;
        });
    
        $.get("/api/livro", function (items) {
            $.each(items, function (idx, item) {
                viewModel.Livros.push(new CarrinhoCompras.Livro(item));
            });
        }, "json");
    });