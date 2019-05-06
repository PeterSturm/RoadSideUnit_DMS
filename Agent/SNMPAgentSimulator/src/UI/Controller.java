package UI;

import Agent.RSUAgent;
import Agent.RSUAgentLoader;
import Agent.TrapSender;
import javafx.collections.ObservableList;
import javafx.collections.transformation.FilteredList;
import javafx.event.EventHandler;
import javafx.fxml.FXML;

import javafx.geometry.Insets;
import javafx.scene.Node;
import javafx.scene.control.*;
import javafx.scene.layout.GridPane;
import javafx.event.ActionEvent;

import java.util.ArrayList;

public class Controller {

    @FXML
    private TextArea log;
    @FXML
    private GridPane gridPane;
    @FXML
    private TextField txtPeriodicity;
    @FXML
    private TextField txtminVariance;
    @FXML
    private TextField txtmaxVariance;

    private ArrayList<RSUAgent> rsus;
    private TrapSender trapSender;
    private Thread trapSenderThread;

    public void Controller()
    {
    }



    @FXML
    private void initialize()
    {
        log.appendText("Start load rsu agent from file: " + "rsus.txt\n");
        rsus = RSUAgentLoader.Load("rsus.txt");
        for (int i = 0; i < RSUAgentLoader.processedLines.size(); i++) {
            if (!RSUAgentLoader.processedLines.get(i))
            log.appendText(String.format("Line %d failed to load\n", i));
        }

        Button btnStartAll = new Button("Start All");
        btnStartAll.setId("btnStartAll");
        btnStartAll.setOnAction(AllAgentStart);
        gridPane.add(btnStartAll, 2, 0);

        Button btnStartSendAll = new Button("Start Send All");
        btnStartSendAll.setId("btnStartAll");
        btnStartSendAll.setOnAction(AllAgentStartSend);
        gridPane.add(btnStartSendAll, 3, 0);

        for (int i = 0; i < rsus.size(); i++)
        {
            Label rsulabel = new Label(rsus.get(i).IP+"/"+rsus.get(i).Port);
            rsulabel.setId(String.format("rsulabel_%d", i));
            gridPane.add(rsulabel, 0, i+1);

            Label rsuIsRealLabel = new Label((rsus.get(i).Real)? "Real" : "Not real");
            rsuIsRealLabel.setId(String.format("rsuislabel_%d", i));
            gridPane.add(rsuIsRealLabel, 1, i+1);

            Button rsubtnStart = new Button("Start");
            rsubtnStart.setId(String.format("btnstart_%d", i));
            rsubtnStart.setOnAction(rsuAgentStart);
            gridPane.add(rsubtnStart, 2, i+1);

            Button rsubtnSend = new Button("Start Send Trap");
            rsubtnSend.setId(String.format("btnsend_%d", i));
            rsubtnSend.setOnAction(rsuAgentSendTrap);
            rsubtnSend.setDisable(true);
            gridPane.add(rsubtnSend, 3, i+1);


            log.appendText("Loaded " + rsus.get(i).IP + "/" + rsus.get(i).Port + " RSU agent\n");
        }

        log.appendText("Finished loading rsus\n");

        txtPeriodicity.setOnAction(changePeriodicity);
        txtPeriodicity.setText("10000");
        txtminVariance.setOnAction(changeMinVariance);
        txtminVariance.setText("-1500");
        txtmaxVariance.setOnAction(changeMaxVariance);
        txtmaxVariance.setText("1500");

        trapSender = new TrapSender(rsus, log, Integer.parseInt(txtPeriodicity.getText()), Integer.parseInt(txtminVariance.getText()), Integer.parseInt(txtmaxVariance.getText()));
        trapSenderThread = new Thread(trapSender);
        trapSenderThread.start();

    }

    public void shutdown()
    {
        trapSender.shutdown();
        for(RSUAgent rsuAgent : rsus)
        {
            if(rsuAgent.Running)
                rsuAgent.stopAgent();
        }
    }

    final EventHandler<ActionEvent> rsuAgentStart = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {
            Button btn = (Button) event.getSource();
            int id = Integer.parseInt(btn.getId().split("_")[1]);

            RSUAgent rsu = rsus.get(id);

            if (!rsu.Running) {
                rsu.startAgent();
                btn.setStyle("-fx-background-color: #00dd00");
                btn.setText("Stop");

                Button btnsend = (Button) gridPane.getChildren().filtered(b -> b.getId().equals(String.format("btnsend_%d", id))).get(0);
                btnsend.setDisable(false);

                log.appendText("Agent " + rsu.IP +"/"+ rsu.Port + " started!\n");
            }
            else {
                if(rsu.SendTrap)
                {
                    rsu.SendTrap = false;
                    Button btnsend = (Button) gridPane.getChildren().filtered(b -> b.getId().equals(String.format("btnsend_%d", id))).get(0);
                    btnsend.setStyle("");
                    btnsend.setText("Start Send Trap");
                    log.appendText("Agent " + rsu.IP+"/"+rsu.Port + " stoped to send trap!\n");
                }
                rsu.stopAgent();
                btn.setStyle("");
                btn.setText("Start");

                Button btnsend = (Button) gridPane.getChildren().filtered(b -> b.getId().equals(String.format("btnsend_%d", id))).get(0);
                btnsend.setDisable(true);

                log.appendText("Agent " + rsu.IP +"/"+ rsu.Port + " stoped!\n");
            }
        }
    };

    final EventHandler<ActionEvent> rsuAgentSendTrap = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {
            Button btn = (Button) event.getSource();
            int id = Integer.parseInt(btn.getId().split("_")[1]);

            RSUAgent rsu = rsus.get(id);

            if (!rsu.SendTrap) {
                rsu.SendTrap = true;
                btn.setStyle("-fx-background-color: #00dd00");
                btn.setText("Stop Send Trap");
                log.appendText("Agent " + rsu.IP+"/"+rsu.Port + " started to send trap !\n");
            }
            else {
                rsu.SendTrap = false;
                btn.setStyle("");
                btn.setText("Start Send Trap");
                log.appendText("Agent " + rsu.IP+"/"+rsu.Port + " stoped to send trap!\n");
            }
        }
    };

    final EventHandler<ActionEvent> changePeriodicity = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {
            TextField txt = (TextField) event.getSource();
            trapSender.setPeriodicity(Integer.parseInt(txt.getText()));
        }
    };

    final EventHandler<ActionEvent> changeMinVariance = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {
            TextField txt = (TextField) event.getSource();
            trapSender.setMinVarinace(Integer.parseInt(txt.getText()));
        }
    };

    final EventHandler<ActionEvent> changeMaxVariance = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {
            TextField txt = (TextField) event.getSource();
            trapSender.setMaxVariance(Integer.parseInt(txt.getText()));
        }
    };

    final EventHandler<ActionEvent> AllAgentStart = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {

            for (Node node : gridPane.getChildren().filtered(b -> b.getId().contains("btnstart_")))
            {
                Button btnstart = (Button) node;
                btnstart.setStyle("-fx-background-color: #00dd00");
                btnstart.setText("Stop");
            }

            for (Node node : gridPane.getChildren().filtered(b -> b.getId().contains("btnsend_")))
            {
                Button btnsend = (Button) node;
                btnsend.setDisable(false);
            }

            rsus.forEach(rsuAgent -> rsuAgent.startAgent());
            log.appendText("All agent started!\n");
        }
    };

    final EventHandler<ActionEvent> AllAgentStartSend = new EventHandler<ActionEvent>()
    {
        @Override
        public void handle(final ActionEvent event) {

            for (int i = 0; i < rsus.size(); i++)
            {
                if(rsus.get(i).Running)
                {
                    int id = i;
                    Button btnsend = (Button) gridPane.getChildren().filtered(b -> b.getId().equals(String.format("btnsend_%d", id))).get(0);
                    btnsend.setStyle("-fx-background-color: #00dd00");
                    btnsend.setText("Stop Send Trap");

                    rsus.get(i).SendTrap = true;
                }
            }

            log.appendText("All started to send trap !\n");
        }
    };
}