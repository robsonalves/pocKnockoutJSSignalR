    var CarrinhoCompras = CarrinhoCompras || {};
    
    CarrinhoCompras.Livro = function (item) {
        var self = this;
        self.id = item.Id;
        self.titulo = item.Titulo;
        self.autor = item.Autor;
        self.quantidade = ko.observable(item.Quantidade);
        self.preco = item.Preco;
        self.quantidadeItensSelecionados = ko.observable(item.QuantidadeItensSelecionados);
    };
    
    CarrinhoCompras.Ordem = function(item) {
        var self = this;
        self.id = item.Id;
        self.aprovadaCompra = ko.observable(item.Aprovado);
        self.nomeCliente = item.NomeCliente;
        self.ordemCompraTotal = item.ValorTotal;
        self.livros = [];
        $.each(item.Livros, function(idx, livro) {
            self.livros.push(new CarrinhoCompras.Livro(livro));
        });
    };