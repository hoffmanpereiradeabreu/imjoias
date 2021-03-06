﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Apresentação.Formulários;

[assembly: ExporBotão(3,
    typeof(Apresentação.Financeiro.ControladorBotãoConsultaRápida),
    Entidades.Privilégio.Permissão.Nenhuma, "Consultar Mercadoria",
    false)]

namespace Apresentação.Financeiro
{
    public class ControladorBotãoConsultaRápida : ControladorBaseInferior
    {
        public override void Exibir()
        {
            ConsultaMercadoria janela;

            janela = new ConsultaMercadoria();
            janela.Show(Formulário);
        }

        protected internal override void AoCarregarCompletamente(Splash splash)
        {
            base.AoCarregarCompletamente(splash);

            Botão.Imagem = Resource.ConsultarMercadoria;
        }
    }
}
